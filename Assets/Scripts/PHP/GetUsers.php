<?php
    define("DB_HOST", "localhost");
    define("DB_USER", "root");
    define("DB_PASSWORD", "");
    define("DB_DATABASE", "eztixunity");
    
    $conn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);
    if ($conn->connect_error)
    {
        die("Connection failed: " . $conn->connect_error);
    }
    
    echo "Connected succesfully!" . "<br><br>";
    
    $sql = "SELECT `Login` FROM `users`";
    $result = $conn->query($sql);
    
    if ($result->num_rows > 0)
    {
        while($row = $result->fetch_assoc())
        {
            echo "Login: " . $row["Login"] . "<br>";
        }
    }
    else
    {
        echo "0 results";
    }
    
    $conn->close();
?>
