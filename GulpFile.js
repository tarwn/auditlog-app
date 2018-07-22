/// <binding BeforeBuild='build' ProjectOpened='local' />

var gulp = require('gulp');
var exit = require('gulp-exit');
var sourcemaps = require('gulp-sourcemaps');
var rename = require('gulp-rename');
var connect = require('gulp-connect');
var colors = require('ansi-colors');
var log = require('fancy-log');

var browserify = require('browserify');
var watchify = require('watchify');
var babelify = require('babelify');
var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var uglify = require('gulp-uglify');

var autoprefixer = require('autoprefixer');
var cssnano = require('cssnano');
var sass = require('gulp-sass');
var postcss = require('gulp-postcss');

var includeKnockoutTemplates = require('gulp-knockout-templates');

var configs = {
    outputFolder: './AuditLogApp/Content/build',
    jsLibInput: './AuditLogApp/Content/src/scripts/lib/*.js',
    jsLibOutput: './AuditLogApp/Content/build/scripts/lib/',
    jsInput: './AuditLogApp/Content/src/scripts/app/app.js',
    cssInput: './AuditLogApp/Content/src/styles/*.css',
    sassInput: ['./AuditLogApp/Content/src/styles/app.scss','./AuditLogApp/Content/src/styles/standalone.scss'],
    sassWatch: './AuditLogApp/Content/src/styles/**/*.scss',
    imagesInput: './AuditLogApp/Content/src/images/**/*.*',
    imagesWatch: './AuditLogApp/Content/src/images/**/*.*',
    imagesOutput: './AuditLogApp/Content/build/images',
    fontsInput: './AuditLogApp/Content/src/font/*',
    fontsOutput: './AuditLogApp/Content/build/font',
    dropinInput: '../auditlog-dropin/build/*',
    dropinOutput: './AuditLogApp/Content/dropin'
};

function applyMinPath(path) {
    path.basename = path.basename + '.min';
}

// JS

function compileJavascript(watch) {
    var bundler = watchify(browserify(configs.jsInput, {
        debug: true,
        insertGlobals: true,
        standalone: 'App'
    }).transform(babelify));

    function rebundle() {
        log('🕒 ', colors.yellow('Building Scripts...'));
        return bundler
            .bundle()
            .on('error', function (err) {
                console.error(err);
                this.emit('end');
            })
            .pipe(source('app.js')) // name is meaningless, but it is used as basename for output
            .pipe(buffer())
            .pipe(rename(applyMinPath))
            .pipe(sourcemaps.init({loadMaps: true}))
            .pipe(uglify())
            .pipe(sourcemaps.write('./'))
            .pipe(gulp.dest(configs.outputFolder + '/scripts/app'));
    }

    if (watch) {
        bundler.on('update', function () {
            console.log('-> bundling...');
            rebundle();
        }).on('time', function (time) {
            log('✅ ', colors.green('Built Scripts in'), colors.cyan(time + 'ms'));
        });

        rebundle();
    }
    else {
        rebundle().pipe(exit());
    }
}

gulp.task('js', function () {
    return compileJavascript(false);
});
gulp.task('js:watch', function () {
    return compileJavascript(true);
});

gulp.task('js-lib', function () { 
    return gulp.src(configs.jsLibInput)
        .pipe(uglify())
        .pipe(gulp.dest(configs.jsLibOutput));
});

// CSS
gulp.task('sass', function(){
    var plugins = [
        autoprefixer(),
        cssnano()
    ];

    return gulp.src(configs.sassInput)
        .pipe(rename(applyMinPath))
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(postcss(plugins))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(configs.outputFolder))
});


gulp.task('sass:watch', function(){
    return gulp.watch(configs.sassWatch, function () {
            // trying to solve for collisions between sass and vs code save
            setTimeout(function () { 
                gulp.start('sass');    
             }, 100)
        })
        .on('change', function(event){
            console.log('[SCSS WATCH] ' + shortenPath(event.path) + ' was ' + event.type);
        });
});

function shortenPath(longPath){
    return longPath.substr(__dirname.length);
}

gulp.task('css-static', function () {
    var plugins = [
        cssnano()
    ];

    return gulp.src(configs.cssInput)
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(postcss(plugins))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(configs.outputFolder));
});

gulp.task('css-static:watch', function () {
    return gulp.watch(configs.cssInput, function () {
        gulp.start('css-static');
    })
        .on('change', function (event) {
            console.log('[CSS WATCH] ' + shortenPath(event.path) + ' was ' + event.type);
        });
});

// Images

gulp.task('images', function(){
    return gulp.src(configs.imagesInput)
        .pipe(gulp.dest(configs.imagesOutput));
});

gulp.task('images:watch', function(){
    return gulp.watch(configs.imagesWatch, ['images'])
        .on('change', function(event){
            console.log('[IMAGE WATCH] ' + shortenPath(event.path) + ' was ' + event.type);
        });
});

// Fonts

gulp.task('fonts', function () { 
    return gulp.src(configs.fontsInput)
        .pipe(gulp.dest(configs.fontsOutput));
});

// DropIn

gulp.task('dropin', function () { 
    return gulp.src(configs.dropinInput)
        .pipe(gulp.dest(configs.dropinOutput));
});
gulp.task('dropin:watch', function () {
    return gulp.watch(configs.dropinInput, ['dropin'])
        .on('change', function(event){
            console.log('[DROPIN WATCH] ' + shortenPath(event.path) + ' was ' + event.type);
        });
});

// Tasks

gulp.task('local', ['images', 'css-static', 'fonts', 'dropin', 'dropin:watch', 'sass', 'images:watch', 'js-lib', 'js:watch', 'sass:watch', 'css-static:watch']);
gulp.task('build', ['images', 'css-static', 'fonts', 'dropin', 'js-lib', 'js', 'sass']);
gulp.task('default', ['local']);