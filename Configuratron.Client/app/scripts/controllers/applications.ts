'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:ApplicationsCtrl
 * @description
 * # ApplicationsCtrl
 * Controller of the ctronApp
 */
angular.module('ctronApp')
 .controller('ApplicationsCtrl', ($scope, $resource, $log) => {
   $scope.applications = [];
   $scope.environments = [];

   var Applications = $resource('http://localhost:2403/applications/:id.json', null, {
                                  'update': { method: 'PUT' }
                                });

   $scope.applications = Applications.query();

   var SettingsOverride = $resource('http://localhost:2403/applications/:appName/settings/:envName/:settingKey.json',
    {appName: '@appName', envName: '@envName', settingKey: '@settingKey'});

   var Environments = $resource('http://localhost:2403/environments/:id.json');
   $scope.environments = Environments.query();

   $scope.updateOverride = (newValue, app, envName, setting) => {
      SettingsOverride.get({ appName: app.name, envName: envName, settingKey: setting.key }, (override) => {
        override.key = setting.key;
        override.value = newValue;
        override.$save({ appName: app.name, envName: envName }, (res) => {
          $log.log('wow... it worked', res);
         });
      });
   };

   $scope.remove = (app) => {
      Applications.remove({id: app.name}, () => {
        $scope.applications = Applications.query();
      });
   };

   $scope.saveSetting = (data) => {
     $log.log('Remove', data);
   };

   $scope.removeSetting = (data) => {
     $log.log('Remove', data);
   };
});
