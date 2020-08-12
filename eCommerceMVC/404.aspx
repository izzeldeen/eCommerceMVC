﻿<% Response.StatusCode = 404 %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <title>404 - Page Not Found</title>
    <meta name="description" content="404 - Page Not Found">
    <meta property="og:title" content="404 - Page Not Found" />
    <meta property="og:description" content="404 - Page Not Found" />
    
    <!--jQuery-->
    <script src="/Scripts/jquery-3.4.1.min.js"></script>

    <!--Bootstrap-->
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="/Scripts/bootstrap.min.js"></script>

    <!--Fontawesome fonts-->
    <link href="/Content/vendor/fontawesome-free-5.9.0-web/css/all.min.css" rel="stylesheet" />
    
    <style>
        .masthead {
            height: 100vh;
            min-height: 500px;
            background-image: url('/Content/images/site/pages/error-page.jpg');
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
        }
    </style>
</head>
<body class="text-center">
    <header class="masthead">
        <div class="container h-100">
            <div class="row h-100 align-items-center">
                <div class="col-12 text-center">
                    <h4 class="display-1">
                        <i class="fas fa-exclamation-triangle"></i>
                    </h4>
                    <h1 class="display-1">
                        404
                    </h1>
                    <p class="lead">
                        Page Not Found.
                    </p>
                    <a href="/">Go to Home</a>
                </div>
            </div>
        </div>
    </header>
</body>
</html>

