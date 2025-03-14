const Dashboard = ()=>{
    var user = JSON.parse(sessionStorage.getItem("UserDetails"));

    var username = user ? user.name : "Guest";

    return (
        <p>
            Dashboard component<br />
            Welcome {username}
        </p>
    );
};

export default Dashboard;