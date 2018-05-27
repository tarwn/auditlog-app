
class HttpError {
    constructor(url, status, error) {
        this.url = url;
        this.status = status;
        this.error = error;
    }
}

function get(url) {
    return new Promise((resolve, reject) => {
        $.get({
            url,
            dataType: 'json',
            success: (data) => {
                resolve(data);
            },
            error: (jqXHR, status, error) => {
                reject(new HttpError(url, jqXHR.status, error));
            }
        });
    });
}

function post(url, data) {
    return new Promise((resolve, reject) => {
        const json = ko.toJSON(data);
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: json,
            dataType: 'json',
            url,
            success: (inData) => {
                resolve(inData);
            },
            error: (jqXHR, status, error) => {
                reject(new HttpError(url, jqXHR.status, error));
            }
        });
    });
}

module.exports = {
    HttpError,
    get,
    post
};
