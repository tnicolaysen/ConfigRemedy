'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:ApplicationDetailsCtrl
 * @description
 * # ApplicationDetailCtrl
 * Controller of the ctronApp
 */
angular.module('ctronApp')
  .controller('ApplicationDetailsCtrl', ($scope, $routeParams, $resource, $log, $window) => {
    $scope.app = {};
    $scope.environments = [];

    var Applications = $resource('http://localhost:2403/applications/:id.json');

    $scope.app = Applications.get({id: $routeParams.appName});

    var SettingsOverride = $resource('http://localhost:2403/applications/:appName/settings/:envName/:settingKey.json',
      null,
      { 'update': { method: 'PUT' } }
    );

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

    $scope.saveSetting = (property, data, app, setting) => {
      $log.log('Save setting', data, app, setting);
      // TODO: bad name

      /* Strict validation
      if (property === 'key' && !data.match(/^(?=\D)\w+/g)) {
        return 'Invalid name. Must match C#\'s property naming grammar.';
      }*/

      var settingToUpdate = angular.copy(setting);
      settingToUpdate[property] = data;
      SettingsOverride.update({ appName: app.name, settingKey: setting.key }, settingToUpdate);
    };

    $scope.removeSetting = (app, setting) => {
      if (!$window.confirm('Are you sure you want to delete the setting ' + setting.key + '?')) {
        return;
      }

      SettingsOverride.remove({ appName: app.name, settingKey: setting.key }, (res) => {
        $scope.app = Applications.get({id: $routeParams.appName});
      });
    };
  });
