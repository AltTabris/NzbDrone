﻿'use strict';
define(
    [
        'backbone',
        'Quality/QualityProfileModel'
    ], function (Backbone, QualityProfileModel) {

        var QualityProfileCollection = Backbone.Collection.extend({
            model: QualityProfileModel,
            url  : window.NzbDrone.ApiRoot + '/qualityprofile'
        });

        var profiles = new QualityProfileCollection();

        profiles.fetch();

        return profiles;
    });
