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
		'ngRoute',
		'ngSanitize',
		'ui.bootstrap',
		'ui.sortable',
		'xeditable'
	])
	.config(($routeProvider, $httpProvider) => {
        $httpProvider.interceptors.push('authInterceptor');
        $routeProvider
			.when('/', {
				templateUrl: 'views/main.html',
				controller: 'MainCtrl'
			})
			.when('/about', {
				templateUrl: 'views/about.html',
				controller: 'AboutCtrl'
			})
			.when('/environments', {
				templateUrl: 'views/environments.html',
				controller: 'EnvironmentsCtrl'
			})
		  .when('/applications', {
				templateUrl: 'views/applications.html',
				controller: 'ApplicationsCtrl'
			})
          .when('/configuration/diagnostics', {
				templateUrl: 'views/diagnostics.html',
				controller: 'DiagnosticsCtrl'
			})
		  .when('/applications/:appName', {
				templateUrl: 'views/application-details.html',
				controller: 'ApplicationDetailsCtrl'
			})
          .when('/login', {
				templateUrl: 'views/login.html',
				controller: 'LoginController'
			})
		  .otherwise({
				redirectTo: '/'
			});
	})
	.run(function(editableOptions, editableThemes) {
		editableOptions.theme = 'bs3';
		editableThemes.bs3.inputClass = 'input-sm';
		editableThemes.bs3.buttonsClass = 'btn-sm';
	});
