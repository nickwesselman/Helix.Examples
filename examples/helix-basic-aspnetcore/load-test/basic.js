import http from 'k6/http';
import { sleep, check } from 'k6';
import { Counter } from 'k6/metrics';

// A simple counter for http requests

export const requests = new Counter('http_reqs');

// you can specify stages of your test (ramp up/down patterns) through the options object
// target is the number of VUs you are aiming for

export const options = {
    scenarios: {
        userPath: {
            executor: 'constant-vus',
            exec: 'userPath',
            vus: 50,
            duration: '15s',
        },
    }
};

export function userPath() {
  var res = http.get('https://netcore-edge-test.azurewebsites.net/en/');
  check(res, {
    'is status 200': (r) => r.status === 200,
  });
  res = http.get('https://netcore-edge-test.azurewebsites.net/en/Products');
  check(res, {
    'is status 200': (r) => r.status === 200,
  });
  res = http.get('https://netcore-edge-test.azurewebsites.net/en/Services');
  check(res, {
    'is status 200': (r) => r.status === 200,
  });
}

export function noEdgePath() {
    // Edge not used on error path
    var res = http.get('https://netcore-edge-test.azurewebsites.net/Error');
    check(res, {
        'is status 200': (r) => r.status === 200,
        });
}