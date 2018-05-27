export default class PageDefinition {
    constructor(title, activeMenuItem, route, component) {
        this.title = title;
        this.activeMenuItem = activeMenuItem;
        this.route = route;
        this.component = component;
    }
}
