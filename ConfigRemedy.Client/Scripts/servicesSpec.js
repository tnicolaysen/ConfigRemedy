/// <reference path="services.ts" />
'use strict';
describe('Services', function () {
    beforeEach(module('app.services'));

    describe('version', function () {
        it('should return current version', inject(function (version) {
            expect(version).toEqual('1.0.0');
        }));
    });
});
//# sourceMappingURL=servicesSpec.js.map
