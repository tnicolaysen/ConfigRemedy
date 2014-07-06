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
    'ui.bootstrap'
  ])
  .config(($routeProvider) => {
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
      .otherwise({
        redirectTo: '/'
      });
  });
