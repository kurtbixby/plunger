import 

function SearchDropDown(props) {
    const { labelText, name, type, value } = props;
    
    function onChange(event) {
        // editValue(e.target.name, e.target.value);
        // update value
        // length check for presence of dropdown
        // make request
        // fill in/build dropdown with results
    }
    
    return (
        <div>
            <label htmlFor={name}>{labelText}</label>
            <input
                name={name}
                type={type}
                value={value}
                onChange={(e) => onChange(e)}
            />
        </div>
    )
}