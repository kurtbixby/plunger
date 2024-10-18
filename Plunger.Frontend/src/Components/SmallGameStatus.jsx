import GameStatusWidget from "../Widgets/GameStatusWidget.jsx";
import {createCoverUrl} from "../Utils.js";

function SmallGameStatus(props) {
  const {
    game: { game, status },
  } = props;

  return (
    <div className="grid grid-cols-2">
      <img src={createCoverUrl(game.coverImageId)} alt="" />
      <GameStatusWidget game={{ game, status }} />
    </div>
  );
}

export default SmallGameStatus;
