function SmallRowElement(props) {
    const {label, body} = props;

    return <div className="grow flex flex-col content-start items-center">
        <p className="text-center">{label}</p>
        <p className="text-center">{body}</p>
    </div>
}

export default SmallRowElement;