import ajax from './ajax';

export default class ServiceDirectory {
    constructor() {
        this.basePath = '/api/appln/v1';
    }

    // Company Configurations

    createAPIKey(displayName) {
        return ajax.post(`${this.basePath}/configuration/apiKeys/create`, { displayName });
    }

    getAPIKeys() {
        return ajax.get(`${this.basePath}/configuration/apiKeys`);
    }

    revokeAPIKey(customerAuthenticationId) {
        return ajax.post(`${this.basePath}/configuration/apiKeys/${customerAuthenticationId}/revoke`);
    }

    // Customized Views

    getOrCreateDefaultView() {
        return ajax.get(`${this.basePath}/customization/views/default`);

        //return ajax.fakeGet(`${this.basePath}/customization/views/default`, () => {
        //    return {
        //        id: '1234567890',
        //        custom: {
        //            url: 'https://www.audotlog.co/',
        //            logo: 'images/logo56-alt3.png',
        //            title: 'auditlog',
        //            headerLinks: [
        //                { url: 'https://app.auditlog.co', label: 'Home' },
        //                { url: 'mailto://contact@auditlog.co', label: 'Contact' }
        //            ],
        //            copyright: 'Copyright AuditLog.co, 2018, All rights reserved.'
        //        },
        //        columns: [
        //            /* eslint-disable */
        //            { order: 0, label: 'Event', lines: [{ field: 'action' }, { field: 'description' }] },
        //            { order: 1, label: 'Actor', lines: [{ field: 'actor.email' }] },
        //            { order: 2, label: 'Source', lines: [{ field: 'context.client.ip_address' }] },
        //            { order: 3, label: 'Item Changed', lines: [{ field: 'target.type' }, { field: 'target.label' }] },
        //            { order: 4, label: 'Time', lines: [{ field: 'time[time]' }] }
        //            /* eslint-enable */
        //        ]
        //    };
        //});
    }

    saveView(view) {
        return ajax.post(`${this.basePath}/configuration/views/default`, view);
        //return ajax.fakePost(`${this.basePath}/configuration/views/default`, view);
    }

    resetViewKey(viewId) {
        return aja.post(`${this.basePath}/configuration/views/default/resetKey`, viewId);

        //return ajax.fakePost(`${this.basePath}/configuration/views/default/resetKey`, viewId)
        //    .then(() => {
        //        return 'abc-123';
        //    });
    }
}
