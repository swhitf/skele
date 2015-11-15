exports = function () {

    var id = 10000;
    
    Default.For('.*\.Id', function (column) {
        console.dir(column);
        return id++;
    });

    Default.For('.*\.CreatedAt', function () {
        return new Date();
    });

};