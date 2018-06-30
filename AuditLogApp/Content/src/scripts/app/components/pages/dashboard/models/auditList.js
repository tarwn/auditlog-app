import NavigationLinkModel from './navigationLink';

export default class AuditListModel {
    constructor(rawAuditList) {
        this.currentLabel = ko.observable();
        this.nextLink = ko.observable();
        this.previousLink = ko.observable();
        this.entries = ko.observableArray();

        this.update(rawAuditList);
    }

    update(rawAuditList) {
        this.currentLabel(rawAuditList._id.label);
        this.nextLink(new NavigationLinkModel(rawAuditList._links.next));
        this.previousLink(new NavigationLinkModel(rawAuditList._links.previous));

        const entries = rawAuditList.entries.map(rawEntry => rawEntry);
        this.entries(entries);
    }
}
