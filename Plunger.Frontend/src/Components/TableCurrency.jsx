import CurrencyInput from "./CurrencyInput.jsx";
import {formatCurrency} from "../Utils.js";

function TableCurrency(props) {
    const {styles, name, value, onChange, isEditable} = props;
    return !isEditable ? <div>
        <p>{name}</p>
        <p>{formatCurrency(value)}</p>
    </div> : <div>
        <label className="block" htmlFor={name.toLowerCase()}>{name}</label>
        <CurrencyInput name={name.toLowerCase()} value={value} onChange={onChange}/>
    </div>
}

export default TableCurrency;