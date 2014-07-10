'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:ApplicationDetailsCtrl
 * @description
 * # ApplicationDetailCtrl
 * Controller of the ctronApp
 */
angular.module('ctronApp')
  .controller('ApplicationDetailsCtrl', ($scope, $routeParams, $resource, $log) => {
    $scope.app = {};
    $scope.environments = [];

    var Applications = $resource('http://localhost:2403/applications/:id.json');

    $scope.app = Applications.get({id: $routeParams.appName});

    var SettingsOverride = $resource('http://localhost:2403/applications/:appName/settings/:envName/:settingKey.json',
      { appName: '@appName', envName: '@envName', settingKey: '@settingKey' });

    var Environments = $resource('http://localhost:2403/environments/:id.json');
    $scope.environments = Environments.query();
  });
