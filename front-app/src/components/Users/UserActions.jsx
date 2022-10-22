import { TableCell } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import SvgIcon from "@mui/material/SvgIcon";
import DeleteIcon from "@mui/icons-material/Delete";
import { Button } from "@mui/material";
const UserActions = ({ handleSelected }) => {
  return (
    <TableCell align="right">
      <Button onClick={handleSelected}>
        <SvgIcon>
          <EditIcon />
        </SvgIcon>
      </Button>
    </TableCell>
  );
};

export default UserActions;
