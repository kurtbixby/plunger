function TableCheckbox(props) {
    const {styles, name, value, onChange, isEditable} = props;
    
    return <div>
        <label className="block" htmlFor={name.toLowerCase()}>{name}</label>
        <input type="checkbox" onChange={onChange} name={name.toLowerCase()} checked={!!value} disabled={!isEditable}/>
    </div>
}

export default TableCheckbox;