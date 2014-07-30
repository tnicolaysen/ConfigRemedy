/// <reference path='../typings/_all.d.ts' />
'use strict';

/**
 * @ngdoc overview
 * @name ctronApp
 * @description
 * # ctronApp
 *
 * Main module of the application.
 */
angular
	.module('ctronApp', [
		'ngAnimate',
		'ngCookies',
		'ngResource',
		'ngSanitize',
		'ui.bootstrap',
		'ui.sortable',
		'ui.router',
		'xeditable'
	])
	.config(function($httpProvider, $stateProvider, $urlRouterProvider, UserRoles){
        $httpProvider.interceptors.push('authInterceptor');

        $stateProvider
            .state('main', {
                url: '/',
                templateUrl: 'views/main.html',
                controller: 'MainCtrl',
                data: {
                    authorizedRoles: [UserRoles.guest, UserRoles.admin]
                }
            })
            .state('about', {
                url: "/about",
                templateUrl: 'views/about.html',
                controller: 'AboutCtrl',
                data: {
                    authorizedRoles: [UserRoles.guest, UserRoles.admin]
                }
            })
            .state('environments', {
                url: "/environments",
                templateUrl: 'views/environments.html',
                controller: 'EnvironmentsCtrl',
                data: {
                    authorizedRoles: [UserRoles.admin]
                }
            })
            .state('applications', {
                url: "/applications",
                templateUrl: 'views/applications.html',
                controller: 'ApplicationsCtrl',
                data: {
                    authorizedRoles: [UserRoles.admin]
                }
            })
            .state('diagnostics', {
                url: "/diagnostics",
                templateUrl: 'views/diagnostics.html',
                controller: 'DiagnosticsCtrl',
                data: {
                    authorizedRoles: [UserRoles.admin]
                }
            })
            .state('login', {
                url: "/login",
                templateUrl: 'views/login.html',
                controller: 'LoginController',
                data: {
                    authorizedRoles: [UserRoles.guest]
                }
            });
	})

	.run(function(editableOptions, editableThemes) {
		editableOptions.theme = 'bs3';
		editableThemes.bs3.inputClass = 'input-sm';
		editableThemes.bs3.buttonsClass = 'btn-sm';
    })
    .run(function($rootScope, $log, $state, AuthenticationEvents, AuthService) {
        $rootScope.$on('$stateChangeStart', function (event, next) {

            var authorizedRoles = next.data.authorizedRoles;
            if (AuthService.isAuthenticated()) {
                if (!AuthService.isAuthorized(authorizedRoles)) {
                    event.preventDefault();
                    $rootScope.$broadcast(AuthenticationEvents.notAuthorized);
                }
            } else {
                // user is not logged in
                $rootScope.$broadcast(AuthenticationEvents.notAuthenticated);
                if (!AuthService.isGuest(authorizedRoles))
                {
                    event.preventDefault();
                    $state.go('login');
                }
            }
        });
	});


