/// <reference path="filters.ts" />
'use strict';
describe('Filters', function () {
    beforeEach(module('app.filters'));

    describe('interpolate', function () {
        beforeEach(module(function ($provide) {
            $provide.value('version', 'TEST_VER');
        }));

        it('should replace VERSION', inject(function (interpolateFilter) {
            expect(interpolateFilter('before %VERSION% after')).toEqual('before TEST_VER after');
        }));
    });
});
//# sourceMappingURL=filtersSpec.js.map
