"use strict";
var router_1 = require("@angular/router");
var login_component_1 = require("./component/login/login.component");
var welcome_component_1 = require("./component/welcome/welcome.component");
var activation_guard_1 = require("./services/guard/activation-guard");
exports.AppRoutes = [
    { path: 'login', component: login_component_1.LoginComponent },
    { path: '', component: login_component_1.LoginComponent },
    { path: 'welcome', component: welcome_component_1.WelcomeComponent, canActivate: [activation_guard_1.AuthGuardService] }
];
exports.ROUTING = router_1.RouterModule.forRoot(exports.AppRoutes);
//# sourceMappingURL=app.routing.js.map