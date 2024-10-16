import GameStatusWidget from "../Widgets/GameStatusWidget.jsx";

function SmallGameStatus(props) {
  const {
    game: { game, status },
  } = props;

  return (
    <div className="grid grid-cols-2">
      <img src={game.coverUrl} alt="" />
      <GameStatusWidget game={{ game, status }} />
    </div>
  );
}

export default SmallGameStatus;
