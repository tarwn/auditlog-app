import ServiceDirectory from './services/serviceDirectory';
import SiteWideViewModel from './viewmodels/sitewide';
import AppViewModel from './viewmodels/app';

import PageDefinition from './viewmodels/pages/pageDefinition';
import HomePage from './components/pages/dashboard/homePage';
import APIKeysListPage from './components/pages/apikeys/listAPIKeysPage';
import APIKeysCreatePage from './components/pages/apikeys/createAPIKeysPage';
import ViewEditPage from './components/pages/views/editViewPage';

export default {
    create: () => {
        const services = new ServiceDirectory();
        const context = new SiteWideViewModel(services);
        const viewmodel = new AppViewModel(services, context);

        // match the binding + URL from ApplicaitonLayout.cshtml
        viewmodel.addPage(new PageDefinition('Home', 'home', '/', HomePage));
        viewmodel.addPage(new PageDefinition('API Keys', 'apikeys', '/configure/apikeys', APIKeysListPage));
        viewmodel.addPage(new PageDefinition('Create API Keys', 'apikeys', '/configure/apikeys/create', APIKeysCreatePage));
        viewmodel.addPage(new PageDefinition('Customize Client View', 'customize', '/customize/view', ViewEditPage));
        viewmodel.mount();

        return viewmodel;
    }
};
