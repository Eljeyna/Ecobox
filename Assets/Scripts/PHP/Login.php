<?php
    $servername = "localhost";
    $username = "root";
    $password = "";
    $dbname = "eztixunity";
    
    $userLogin = $_POST["userLogin"];
    $userPassword = $_POST["userPassword"];
    
    $conn = new mysqli($servername, $username, $password, $dbname);
    if ($conn->connect_error)
    {
        die("Connection failed: " . $conn->connect_error);
    }

    $sql = "SELECT `ID`, `Password` FROM `users` WHERE `Login` = '" . $userLogin . "'" . "OR `Email` = '" . $userLogin . "'";
    $result = $conn->query($sql);

    if ($result->num_rows > 0)
    {
        while ($row = $result->fetch_assoc())
        {
            if ($row["Password"] === $userPassword)
            {
                echo $row["ID"];
            }
            else
            {
                echo "Wrong password";
            }
        }
    }
    else
    {
        echo "User does not exists";
    }
    
    $conn->close();
?>
