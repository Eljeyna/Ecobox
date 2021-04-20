<?php
    $servername = "localhost";
    $username = "root";
    $password = "";
    $dbname = "eztixunity";
    
    $userLogin = $_POST["userLogin"];
    $userEmail = $_POST["userEmail"];
    $userPassword = $_POST["userPassword"];
    
    $conn = new mysqli($servername, $username, $password, $dbname);
    if ($conn->connect_error)
    {
        die("Connection failed: " . $conn->connect_error);
    }
    
    $sql = "SELECT `Password`, `Email` FROM `users` WHERE `Login` = '" . $userLogin . "'" . "OR `Email` = '" . $userEmail . "'";
    $result = $conn->query($sql);
    
    if ($result->num_rows > 0)
    {
        echo "Login/Email already taken";
    }
    else
    {
        $sqlRegister = "INSERT INTO `users` (`Login`, `Email`, `Password`) VALUES ('" . $userLogin . "', '" . $userEmail . "', '" . $userPassword . "')";

        if ($conn->query($sqlRegister) === true)
        {
            echo "All done";
        }
        else
        {
            echo "Error: " . $sqlRegister . "<br>" . $conn->error;
        }
    }
    
    $conn->close();
?>
