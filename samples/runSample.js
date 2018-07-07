/* eslint-disable */
const fs = require("fs");
const http = require("http");

const host = process.argv[2];
const port = process.argv[3];
const auth = process.argv[4];
const startDateAdjust = process.argv[5] || 0;

const targetStartDate = new Date(Date.now() + (startDateAdjust * 24 * 60 * 60 * 1000));

const content = fs.readFileSync("sampleEvents.json");
const events = JSON.parse(content).sort((a,b) => { 
    return a.time - b.time;
});
const targetEventCount = Math.round(Math.random() * events.length);
 
//fix up dates
const initialDate = events[0].time * 1000;
const lastDate = events[events.length - 1].time * 1000;
const dateDiff = targetStartDate - lastDate;

console.log('------');
console.log('Data will be for: ' + new Date(lastDate + dateDiff).toUTCString());
console.log('Number of events: ' + targetEventCount + '/' + events.length);
console.log('------');

events.forEach((e) => { 
    e.time = new Date(e.time * 1000 + dateDiff).toUTCString();
});

// remove some random count of events
const eventsToSend = events.slice(0, targetEventCount);

console.log("Calling api...");
const postData = JSON.stringify(eventsToSend);

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
