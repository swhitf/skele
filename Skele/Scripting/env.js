var require = function (filePath) {
    console.log("require: " + filePath);
    return (function () {
        var module = {};
        var exports = module.exports = {};
        eval(file(filePath));
        return module.exports;
    })();
};