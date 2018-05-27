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
}
