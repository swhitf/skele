exports = function (seed) {

    var id = seed || 1000;
    
    Default.For('.*\.Id', function (column) {
        console.dir(column);
        return id++;
    });

    Default.For('.*\.CreatedAt', function () {
        return new Date();
    });

};