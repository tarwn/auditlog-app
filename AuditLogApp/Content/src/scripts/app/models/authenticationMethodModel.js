export default class AuthenticationMethodModel {
    constructor(rawData) {
        this.id = rawData.id;
        this.secret = rawData.secret;
        this.credentialType = rawData.credentialType;
        this.displayName = rawData.displayName;
        this.creationTime = rawData.creationTime;
        this.isRevoked = rawData.isRevoked;
        this.revokeTime = rawData.revokeTime;

        this.display = {
            creationTime: 'pretty string',
            revokeTime: 'other string'
        };
    }
}
