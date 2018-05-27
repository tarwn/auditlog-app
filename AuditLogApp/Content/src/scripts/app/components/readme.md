Creating Components
=====================================

1. Create the component:

```javascript
export default {
    name: 'modal-create-apikey',
    viewModel: class CreateAPIKeyModal {
        constructor(params) { 
            this.doThingsWithBoundParams = params.whatever;
        }
    },
    template: `
        <h1>Stuff here</h1>
    `
};
```

2. Register it

* If it's a page, add it with a rout in app/app.js
* If not, add it to app/components/allComponents.js
