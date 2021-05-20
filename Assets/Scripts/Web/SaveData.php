<?php
    $servername = "localhost";
    $username = "root";
    $password = "";
    $dbname = "eztixunity";
    
    $userID = $_POST["userID"];
    $userData = $_POST["userData"];
    
    $conn = new mysqli($servername, $username, $password, $dbname);
    if ($conn->connect_error)
    {
        die("Connection failed: " . $conn->connect_error);
    }

    $sql = "UPDATE `users` SET `SaveData` = '" . $userData . "' WHERE `ID` = '"$userID"'";
    if ($conn->query($sql) === TRUE)
    {
        echo "All done";
    }
    else
    {
        echo "Error :( " . $conn->error;
    }

    $conn->close();
?>
