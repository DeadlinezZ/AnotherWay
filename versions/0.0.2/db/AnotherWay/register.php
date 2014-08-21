<?php
require_once("db.php");

//Create db object
@$db = new mysqli($host, $user, $pass, $database);

//Check conenction
if(!$db){
	die("Failed to connect to Server");
}

$secretKey = "YoloSwagHose";
$unityHash = $_POST['hash'];
$username = $_POST['username'];
$password = $_POST['password'];
$password2 = $_POST['password2'];
$email = $_POST['email'];
$phpHash = md5($username . $email . $password . $password2 . $secretKey);

//Validate data
validate($db, $unityHash, $phpHash, $username, $password, $password2, $email);

//Register the user
register($db, $username, md5($password), $email);

$db->close();

function validate($db, $hash1, $hash2, $username, $password, $password2, $email){
	//Check if the hashes do match
	if($hash1 != $hash2){
		die("HASH code is diferent from your game, you infidel.");
	} 

	//Check if there are empty fields
	if(empty($username) || empty($password) || empty($password2) || empty($email)){
		die("There are empty fields!");
	} 

	if(!(preg_match("/^[a-zA-Z\d ]+$/", $username))){
		die("Only numbers and letters are allowed in username.");
	}	

	//Check if username is between 4 and 20 chars
	if(strlen($username) < 4 || strlen($username) > 20){
		die("Username must be between 4 and 20 chars long.");
	}
		
	//Check if the Email is valid
	if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
		die("Invalid Email-Adress.");
	}  
		
	//Check if password has over 4 chars
	if(strlen($password) < 4){
		die("Password must be 4 chars or longer.");
	}
		
	//Check if the two passwords are equal
	if($password != $password2){
		die("The two passwords are not equal.");
	} 

	//Check if user is free
	checkUser($db, $username);

	//Check if email is already used
	checkEmail($db, $email);
}

function checkUser($db, $username){
	//Check if the user exists
	$stmt = $db->prepare("SELECT username FROM users WHERE username LIKE (?)");
	if(!$stmt){
		die("Something went wrong.");
	}
	$stmt->bind_param("s", $username);
	if(!$stmt->execute()){
		die("Something went wrong.");
	}
	$stmt->store_result();
	$count = $stmt->num_rows;
	$stmt->close();
												
	if($count > 0){
		die("This username has already been taken.");
	} 
}

function checkEmail($db, $email){
	//Check if the email exists
	$stmt = $db->prepare("SELECT email FROM users WHERE email LIKE (?)");
	if(!$stmt){
		die("Something went wrong.");
	}
	$stmt->bind_param("s", $email);
	if(!$stmt->execute()){
		die("Something went wrong.");
	}
	$stmt->store_result();
	$count = $stmt->num_rows;
	$stmt->close();
												
	if($count > 0){
		die("This Email-Adress has already been used.");
	} 
}

function register($db, $username, $password, $email){
	//Register Query
	$stmt = $db->prepare("INSERT INTO users (username,password,email) VALUES(?,?,?)");
	if(!$stmt){
		die("Something went wrong.");
	}
	$stmt->bind_param("sss", $username, $password, $email);
	if(!$stmt->execute()){
		die("Something went wrong.");
	}
	$stmt->close();
	echo("1"); //Successfully registered
}



?>