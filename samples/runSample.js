/* eslint-disable */
const fs = require("fs");
const http = require("http");

const host = process.argv[2];
const port = process.argv[3];
const auth = process.argv[4];

const content = fs.readFileSync("sampleEvents.json");
const events = JSON.parse(content).sort((a,b) => { 
    return a.time - b.time;
});
 
//fix up dates
const initialDate = events[0].time * 1000;
const lastDate = events[events.length - 1].time * 1000;
const dateDiff = Date.now() - lastDate;

console.log('------');
console.log(new Date(lastDate).toUTCString());
console.log(new Date(lastDate + dateDiff).toUTCString());
console.log('------');

events.forEach((e) => { 
    e.time = new Date(e.time * 1000 + dateDiff).toUTCString();
});

console.log("Calling api...");
const postData = JSON.stringify(events);

const
    options = {
    host: host,
    port: port,
    path: '/api/v1/events',
    method: 'POST',
    headers: {
        'X-API-KEY': auth,
        'Content-Type': 'application/json',
        'Content-Length': Buffer.byteLength(postData)
    }
  };
  
const req = http.request(options, (res) => {
    let response = '';

    console.log('STATUS: ' + res.statusCode);
    console.log('HEADERS: ' + JSON.stringify(res.headers));
    res.setEncoding('utf8');
    res.on('data', function (chunk) {
        response += chunk;
    });
    res.on('end', () => {
        console.log(JSON.parse(response));

        console.log('No more data in response.');
    });  
});

req.on('error', (e) => {
    console.error(`problem with request: ${e.message}`);
});

req.write(postData);
req.end();
/* eslint-enable */
