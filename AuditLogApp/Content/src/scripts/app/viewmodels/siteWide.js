export default class SiteWideViewModel {
    constructor(services) {
        this._services = services;

        this.modal = ko.observable();
    }
}
