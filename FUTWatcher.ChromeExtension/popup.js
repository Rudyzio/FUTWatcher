// initialize the constants and variables
var INTERVAL = 7000;
var OPEN_CLUB_DELAY = 3 * 60 * 1000;
var REQUEST_LIMIT = 210;
var MAX_UNAUTHORIZED_COUNT = 1;
var SAVE_URL = "http://localhost:5000/api/Home/Mine";
var BPM_OPENDED_URL = "http://localhost:5000/Home/BronzePackOpened";
var STATUS_CODES = {
    IDLE: 0,
    WAITING_FOR_REQUEST: 1,
    ACTIVE: 2,
};
var state = {
    version: "1.0.1",
    ea_web_app_opended: false,
    status_code: STATUS_CODES.IDLE,
    timeout: null,
    listening: false,
    url_data_user: null,
    url_data_tradepile: null,
    url_data_cons: null,
    unauthorized_count: 0,
    call_authorized: true
};
// utility functions
function wait(time) {
    return new Promise(function (resolve) {
        setTimeout(resolve, time);
    });
};

function ajax(method, url, headers, body) {
    return new Promise(function (resolve, reject) {
        var forbidden_header_arr = ["origin", "user-agent", "referer", "accept-encoding", "cookie"]; //, "X-UT-SID".toLowerCase() ];
        var xhr = new XMLHttpRequest();
        xhr.open(method, url, true);
        headers.forEach(function (header) {
            if (forbidden_header_arr.indexOf(header.name.toLowerCase()) === -1) {
                xhr.setRequestHeader(header.name, header.value);
            };
        });
        // console.log("Calling AJAX", url, headers, method);
        xhr.onload = function () {
            if (xhr.status >= 200 && xhr.status < 400) {
                try {
                    resolve(JSON.parse(xhr.responseText));
                } catch (e) {
                    reject(xhr.responseText);
                };
            } else {
                try {
                    resolve(JSON.parse(xhr.responseText));
                } catch (e) {
                    reject(xhr.responseText);
                };
            };
        };
        xhr.onerror = function (error) {
            try {
                resolve(JSON.parse(xhr.responseText));
            } catch (e) {
                reject(xhr.responseText);
            };
        };
        xhr.send(body);
    });
};

function get_card_data(url, headers) {
    return ajax("GET", url, headers, " ");
};

function get_user_data(host, url_data, headers) {
    if (url_data.timestamp == 1) {
        url_data.url = url_data.url + "?_=" + Date.now();
    }
    return ajax(url_data.method, "https://" + host + url_data.url, headers, " ");
};

function get_tradepile_data(host, url_data, headers) {
    if (url_data.timestamp == 1) {
        url_data.url = url_data.url + "?_=" + Date.now();
    }
    return ajax(url_data.method, "https://" + host + url_data.url, headers, " ");
};

function get_cons_data(host, url_data, headers) {
    if (url_data.timestamp == 1) {
        url_data.url = url_data.url + "?_=" + Date.now();
    }
    return ajax(url_data.method, "https://" + host + url_data.url, headers, " ");
};

function submit_data(data) {
    var headers = [{
        name: "Content-Type",
        value: "application/json",
    }];
    var body = {
        message: JSON.stringify(data)
    }
    body = JSON.stringify(body);
    return new Promise(function (resolve, reject) {
        ajax("POST", SAVE_URL, headers, body)
            .then(function (response) {
                console.log(response);
                resolve(response);
            })
    });
};

function run(url, headers, start) {

    var params = "?sort=desc&type=player&defId=&start={{start}}&count=91&_=" + Date.now();
    var url = url.replace("tradepile", "club");
    var url = url.replace("watchlist", "club");
    var flag;
    params = params.replace("{{start}}", start);

    wait(INTERVAL)
        .then(function () {
            return get_card_data(url + params, headers)
        })
        .then(function (card_data) {
            if (card_data.code && card_data.code === 401) {
                state.unauthorized_count += 1;
                if (state.unauthorized_count > MAX_UNAUTHORIZED_COUNT) {
                    state.status_code = STATUS_CODES.IDLE;
                    console.log("network_error_ea_expired", state);
                } else {
                    state.listening = true;
                    state.status_code = STATUS_CODES.WAITING_FOR_REQUEST;
                    console.log("unauthorized", state);
                }
            } else if (card_data.code) {
                state.status_code = STATUS_CODES.IDLE;
                console.log("network_error_ea", state);
            } else {
                if ((start / 91) + 1 === REQUEST_LIMIT) { // limit exceeded
                    flag = 4;
                } else if (start === 0 && card_data.itemData.length < 91) { // start and end
                    flag = 3;
                } else if (start === 0) { // start
                    flag = 0;
                } else if (card_data.itemData.length < 91) { // end
                    flag = 1;
                } else { // normal
                    flag = 2;
                };
                // console.log(card_data);
                submit_data(card_data)
                    .then(function (response) {
                        if (flag === 0 || flag === 2) {
                            run(url, headers, start + 91);
                        } else {
                            state.status_code = STATUS_CODES.IDLE;
                            console.log("finished_fetching", state);
                            wait(INTERVAL)
                        };
                    })
                    .catch(function () {
                        state.status_code = STATUS_CODES.IDLE;
                        console.log("network_error", state);
                    });
            };
        })
        .catch(function (status_code) {
            state.status_code = STATUS_CODES.IDLE;
            console.log("network_error_ea", state);
        });
};
// define event listener functions
function web_request_listener(details) {
    console.log("web_request", details);
    var native_request = false;
    if (details.method == "GET") {
        details.requestHeaders.forEach(function (header) {
            var name = header.name.toLowerCase();
            if (name === "origin") {
                if (header.value === "https://www.easports.com") {
                    native_request = true;
                } else {
                    header.value = "https://www.easports.com";
                };
            };
        });

        if (native_request === false) {
            details.requestHeaders.push({
                name: "Referer",
                value: "https://www.easports.com/uk/fifa/ultimate-team/web-app/"
            });
        };

        if (native_request && state.listening && state.call_authorized) {
            state.listening = false;
            clearTimeout(state.timeout);
            state.status_code = STATUS_CODES.ACTIVE;
            console.log("started_fetching", state);
            var headers = [{
                name: "Content-Type",
                value: "application/json",
            }];
            makeRequest("GET", "http://localhost:5000/api/Home/ResetOwned", headers, null);
            run(details.url.split("?")[0], details.requestHeaders, 0);
        };
        return {
            requestHeaders: details.requestHeaders,
        };
    }


};

function tabs_updated_listener() {
    chrome.tabs.query({
        url: ["https://www.easports.com/fifa/ultimate-team/web-app/", "https://www.easports.com/*/fifa/ultimate-team/web-app/"]
    }, function (tabs) {
        if (tabs.length === 0) {
            state.ea_web_app_opended = false;
        } else {
            state.ea_web_app_opended = true;
            state.listening = true;
            console.log("WEB APP OPENED");
        };
    });
};

function bronze_pack_listener(details) {
    console.log("Bronze pack opened", details);
    if (details.method == "POST") {
        var requestBodyString = decodeURIComponent(String.fromCharCode.apply(null, new Uint8Array(details.requestBody.raw[0].bytes)));
        var json = JSON.parse(requestBodyString);
        if (json["packId"] == 100) {
            var headers = [{
                name: "Content-Type",
                value: "application/json",
            }];
            var body = {}
            body = JSON.stringify(body);
            makeRequest("POST", BPM_OPENDED_URL, headers, body);
        }
    }
}

function makeRequest(method, url, headers, body) {
    $.ajax({
        url: url,
        headers: headers,
        type: 'post',
        data: body,
        success: function (response) {
            console.log("SUCESSO AO FAZER CHAMADA");
        }
    })
}

// add event listeners
chrome.tabs.onCreated.addListener(tabs_updated_listener);
chrome.tabs.onUpdated.addListener(tabs_updated_listener);
chrome.tabs.onRemoved.addListener(tabs_updated_listener);

// Listen for players from my club
chrome.webRequest.onBeforeSendHeaders.addListener(web_request_listener, {
    urls: ["https://*.fut.ea.com/ut/game/fifa18/club?*"]
}, ['blocking', 'requestHeaders']);

// Listen for bronze pack opening
chrome.webRequest.onBeforeRequest.addListener(bronze_pack_listener, {
    urls: ["https://*.fut.ea.com/ut/game/fifa18/purchased/items"]
}, ['requestBody']);