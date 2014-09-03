<?php

$unityhash = $_POST['hash'];
$username = $_POST['username'];
$password = $_POST['password'];

echo htmlspecialchars($unityhash . " " . $username . " " . $password);

?>
