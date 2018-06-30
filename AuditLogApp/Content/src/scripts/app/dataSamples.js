/* eslint-disable */
export default {
    getEntries: (baseUrl) => {
        const url = 'api/auditLog/view-xyz/20180803';

        return {
            _id: {
                label: 'March 2018',
                type: 'GET',
                href: baseUrl + url,
                timetamp: 'abc'
            },
            _links: {
                next: { label: 'April 2018', type: 'GET', href: 'javascript: alert("Next/Previous not available for sample data")' },
                previous: { label: 'February 2018', type: 'GET', href: 'javascript: alert("Next/Previous not available for sample data")' }
            },
            entries: [
                {
                    "id": "ce35d839-0408-49d6-81f9-be57fea556b1",
                    "uuid": "whatever-you-want",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522313599.8914,
                    "time": 1522313593,
                    "action": "user.login",
                    "description": null,
                    "url": null,
                    "actor": {
                        "uuid": "bd275099-e808-4c1a-bd32-57a589b7cf93",
                        "name": "Roberta Wharton",
                        "email": "rwharton@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.23",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": null,
                        "uuid": null,
                        "label": null,
                        "url": null
                    }
                },
                {
                    "id": "3a44a20a-8bb8-49fd-adcf-b7df5c366532",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522313831.76738,
                    "time": 1522313782,
                    "action": "user.invite",
                    "description": "Invited new user, 'QA' role",
                    "url": "http://blah.example.com/95c102ae-5930-413f-b794-8f0934fdb136",
                    "actor": {
                        "uuid": "bd275099-e808-4c1a-bd32-57a589b7cf93",
                        "name": "Roberta Wharton",
                        "email": "rwharton@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.23",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "user",
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "label": "Salman Randall",
                        "url": "http://blah.example.com/95c102ae-5930-413f-b794-8f0934fdb136"
                    }
                },
                {
                    "id": "5a0f7f24-6222-4aa7-90c8-1cea651d3410",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522314928.54884,
                    "time": 1522314910,
                    "action": "user.register",
                    "description": "Registering from invitation",
                    "url": null,
                    "actor": {
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "name": "Salman Randall",
                        "email": "srandall@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "user",
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "label": "Salman Randall",
                        "url": null
                    }
                },
                {
                    "id": "24942d8f-af8d-412c-a2e8-2982c32670de",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522315002.13906,
                    "time": 1522314990,
                    "action": "user.passwordReset",
                    "description": "Reset own password",
                    "url": null,
                    "actor": {
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "name": "Salman Randall",
                        "email": "srandall@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "user",
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "label": "Salman Randall",
                        "url": null
                    }
                },
                {
                    "id": "11ee8ca7-65cf-47db-8203-c3eafb0c733e",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522315233.79208,
                    "time": 1522315212,
                    "action": "batch.logSamples",
                    "description": "Entered QA samples for batch",
                    "url": null,
                    "actor": {
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "name": "Salman Randall",
                        "email": "srandall@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "batch",
                        "uuid": "5a61a8f0-1390-46bd-a4c7-d5381955d807",
                        "label": "Batch 20180329-003",
                        "url": null
                    }
                },
                {
                    "id": "dd2406e0-50f6-4f76-8e05-8c24dcff4ec4",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522316186.90537,
                    "time": 1522316143,
                    "action": "batch.logSamples",
                    "description": "Entered QA samples for batch",
                    "url": null,
                    "actor": {
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "name": "Salman Randall",
                        "email": "srandall@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "batch",
                        "uuid": "5a61a8f0-1390-46bd-a4c7-d5381955d807",
                        "label": "Batch 20180329-003",
                        "url": null
                    }
                },
                {
                    "id": "947d11b2-a100-4865-a10a-755d24c95d69",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522317101.52425,
                    "time": 1522317043,
                    "action": "batch.logSamples",
                    "description": "Entered QA samples for batch",
                    "url": null,
                    "actor": {
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "name": "Salman Randall",
                        "email": "srandall@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "batch",
                        "uuid": "5a61a8f0-1390-46bd-a4c7-d5381955d807",
                        "label": "Batch 20180329-003",
                        "url": null
                    }
                },
                {
                    "id": "75d604de-efeb-41bc-af60-cc17f21253ad",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522317343.84619,
                    "time": 1522317322,
                    "action": "batch.complete",
                    "description": "Completed batch",
                    "url": null,
                    "actor": {
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "name": "Salman Randall",
                        "email": "srandall@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "batch",
                        "uuid": "5a61a8f0-1390-46bd-a4c7-d5381955d807",
                        "label": "Batch 20180329-003",
                        "url": null
                    }
                },
                {
                    "id": "a89c7f59-3ff0-4226-81c7-f96e328f1093",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522317532.46738,
                    "time": 1522317532,
                    "action": "user.logout",
                    "description": null,
                    "url": null,
                    "actor": {
                        "uuid": "95c102ae-5930-413f-b794-8f0934fdb136",
                        "name": "Salman Randall",
                        "email": "srandall@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": null,
                        "uuid": null,
                        "label": null,
                        "url": null
                    }
                },
                {
                    "id": "41faeeeb-6e4e-472c-a7bb-77ccde084bc2",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522317643.82909,
                    "time": 1522317622,
                    "action": "billing.paymentUpdated",
                    "description": "Updated payment details",
                    "url": null,
                    "actor": {
                        "uuid": "bd275099-e808-4c1a-bd32-57a589b7cf93",
                        "name": "Roberta Wharton",
                        "email": "rwharton@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.23",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": null,
                        "uuid": null,
                        "label": null,
                        "url": null
                    }
                },
                {
                    "id": "152705a2-9189-479a-b09a-e9282ae9887e",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522317653.5092,
                    "time": 1522317645,
                    "action": "billing.paymentVerified",
                    "description": "Payment details verified",
                    "url": null,
                    "actor": {
                        "uuid": "bd275099-e808-4c1a-bd32-57a589b7cf93",
                        "name": "SYSTEM",
                        "email": "SYSTEM"
                    },
                    "context": {
                        "client": {
                            "ipAddress": null,
                            "browserAgent": null
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": null,
                        "uuid": null,
                        "label": null,
                        "url": null
                    }
                },
                {
                    "id": "ea83e8e7-f0a6-4d0e-b29a-c305274399b9",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522318242.20722,
                    "time": 1522318215,
                    "action": "prefs.updateLogo",
                    "description": "Update company logo",
                    "url": null,
                    "actor": {
                        "uuid": "bd275099-e808-4c1a-bd32-57a589b7cf93",
                        "name": "Roberta Wharton",
                        "email": "rwharton@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.23",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": null,
                        "uuid": null,
                        "label": null,
                        "url": null
                    }
                },
                {
                    "id": "dc18c28d-6f7a-4324-8883-7351c2558216",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522321847.56007,
                    "time": 1522321834,
                    "action": "user.login",
                    "description": null,
                    "url": null,
                    "actor": {
                        "uuid": "75d604de-efeb-41bc-af60-cc17f21253ad",
                        "name": "Lilly Romero",
                        "email": "lromero@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": null,
                        "uuid": null,
                        "label": null,
                        "url": null
                    }
                },
                {
                    "id": "720d8737-2726-4f8c-aab6-94238f91eaca",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522322150.78991,
                    "time": 1522322112,
                    "action": "batch.labelPrinted",
                    "description": "Printed label for batch",
                    "url": null,
                    "actor": {
                        "uuid": "75d604de-efeb-41bc-af60-cc17f21253ad",
                        "name": "Lilly Romero",
                        "email": "lromero@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "batch",
                        "uuid": "5a61a8f0-1390-46bd-a4c7-d5381955d807",
                        "label": "Batch 20180329-003",
                        "url": null
                    }
                },
                {
                    "id": "d07656e4-d87d-4b49-b539-b43e47995837",
                    "client": {
                        "id": "1234567890",
                        "name": "Great Client"
                    },
                    "receptionTime": 1522322719.35291,
                    "time": 1522322712,
                    "action": "batch.labelPrinted",
                    "description": "Printed label for batch",
                    "url": null,
                    "actor": {
                        "uuid": "75d604de-efeb-41bc-af60-cc17f21253ad",
                        "name": "Lilly Romero",
                        "email": "lromero@great-client.test"
                    },
                    "context": {
                        "client": {
                            "ipAddress": "192.168.105.68",
                            "browserAgent": "Mozilla/5.0 (Windows NT 6.2; rv:20.0) Gecko/20121202 Firefox/20.0"
                        },
                        "server": {
                            "serverId": "prod-01-us-appfarm",
                            "version": "20180325-01"
                        }
                    },
                    "target": {
                        "type": "batch",
                        "uuid": "5a61a8f0-1390-46bd-a4c7-d5381955d807",
                        "label": "Batch 20180329-003",
                        "url": null
                    }
                }
            ]
        }
    }
}
/* eslint-enable */
