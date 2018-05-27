import ServiceDirectory from './services/serviceDirectory';
import SiteWideViewModel from './viewmodels/sitewide';
import AppViewModel from './viewmodels/app';

modules.export = {
    createServices: () => {
        return new ServiceDirectory();
    },
    createSiteWide: (services) => {
        return new SiteWideViewModel(services);
    },
    createViewModel: (services, siteWide) => {
        return new AppViewModel(services, siteWide);
    }
};