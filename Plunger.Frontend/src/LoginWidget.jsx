import { useContext, useState } from "react";
import { objFromForm } from "./Utils.js";
import API from "./APICalls.js";

function LoginWidget() {
    const [state, setState] = useState({
        username: "",
        password: ""
    });
    
    async function handleSubmit(e) {
        e.preventDefault();

        var formData = objFromForm(e.target);
        var resp = await API.sendLoginRequest({identity: formData.username, password: formData.password});
        console.log(resp);
    }

    function editValue(name, value) {
        setState({
            ...state,
            [name]: value,
        });
    }
    
    return (
        <div>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="username">Username:</label>
                    <input
                        name="username"
                        type="text"
                        value={state.username}
                        onChange={(e) => editValue(e.target.name, e.target.value)}
                    />
                </div>
                <div>
                    <label htmlFor="password">Password:</label>
                    <input
                        name="password"
                        type=""
                        value={state.password}
                        onChange={(e) => editValue(e.target.name, e.target.value)}
                    />
                </div>
                <button type="submit">Login</button>
            </form>
        </div>
    )
}

export default LoginWidget;