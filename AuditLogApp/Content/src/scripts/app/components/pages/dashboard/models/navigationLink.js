export default class NavigationLink {
    constructor(rawLink) {
        this.label = rawLink.label;
        this.url = rawLink.href;
    }
}
