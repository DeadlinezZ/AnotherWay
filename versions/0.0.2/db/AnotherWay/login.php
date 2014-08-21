<?php
require_once("db.php");

//Create db object
@$db = new mysqli($host, $user, $pass, $database);

//Check conenction
if(!$db){
	die("Failed to connect to Login Server");
}

$unityHash = $_POST['hash'];
$username = $_POST['username'];
$password = $_POST['password'];
$secretKey = "YoloSwagHose";
$phpHash = md5($username . $password . $secretKey);

//Validate data
validate($db, $unityHash, $phpHash, $username, $password);

//Login user
login($db, $username, $password);

//Close DB
$db->close();



function validate($db, $hash1, $hash2, $username, $password){
	//Check if the hashes do match
	if($hash1 != $hash2){
		die("HASH code is diferent from your game, you infidel.");
	} 

	//Check if username is between 4 and 20 chars
	if(strlen($username) < 4 || strlen($username) > 20){
		die("Wrong username or password");
	}

	//Check if password has over 4 chars
	if(strlen($password) < 4){
		die("Wrong username or password");
	}

}




function login($db, $username, $password){
	$msg = "Something went wrong.";
	
	//Query user
	$stmt = $db->prepare("SELECT * FROM users WHERE username = (?)");
	if(!$stmt){die($msg);}
	if(!$stmt->bind_param("s", $username)){die($msg);}
	if(!$stmt->execute()){die($msg);}
	if(!$stmt->store_result()){die($msg);}
	if(!$stmt->bind_result($id, $user, $pw, $email, $exp, $ap, $coins, $level, $admin, $gm)){die($msg);}
	
	//Check if user exists (when there are affected rows)
	if(!($stmt->num_rows > 0)){
		die("Wrong username or password");
	}
	while ($stmt->fetch()) {
		//Check if passwords matches
		if(md5($password) != $pw){
			die("Wrong username or password");
		}
		//PW is correct -> Login user
		//The first "1" means true, so the system recognizes that the login was successful
		echo("1," . $id . "," . $user . "," . $email  . "," . $exp . "," . $ap  . "," . $coins . "," . $level . "," . $admin . "," . $gm);
	}
	


	$stmt->close();											
}

?>