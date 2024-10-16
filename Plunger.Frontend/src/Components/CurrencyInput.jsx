import currency from "currency.js";
import { formatCurrency } from "../Utils.js";

function CurrencyInput(props) {
  const { styles, name, value, onChange } = props;

  function tryParsePriceStringLib(priceString) {
    return currency(priceString).intValue;
  }

  function parsePriceString(price) {
    console.log("PriceInput: " + price);
    let priceNum = tryParsePriceStringLib(price);
    console.log("PriceNum: " + price);
    if (isNaN(priceNum)) {
      console.log("Couldn't parse price");
      return null;
    }

    let str = formatCurrency(priceNum);
    console.log("PriceStr: " + str);

    return priceNum;
  }

  return (
    <input
      className={styles ?? ""}
      type="text"
      name={name}
      value={!isNaN(value) ? formatCurrency(value) : formatCurrency(0)}
      onChange={(e) => {
        const result = parsePriceString(e.target.value);
        if (result === null) {
          return;
        }
        e.target.value = result.toString();
        onChange(e);
      }}
    />
  );
}

export default CurrencyInput;
