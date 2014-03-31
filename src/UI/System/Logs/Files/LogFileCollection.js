﻿'use strict';

define(
    [
        'backbone',
        'System/Logs/Files/LogFileModel'
    ], function (Backbone, LogFileModel) {
        return Backbone.Collection.extend({
            url  : window.NzbDrone.ApiRoot + '/log/file',
            model: LogFileModel,

            state: {
                sortKey: 'lastWriteTime',
                order  : 1
            }
        });
    });
