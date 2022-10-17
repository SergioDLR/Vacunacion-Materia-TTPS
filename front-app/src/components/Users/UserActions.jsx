import { TableCell } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import SvgIcon from "@mui/material/SvgIcon";
import DeleteIcon from "@mui/icons-material/Delete";
const UserActions = () => {
  return (
    <TableCell align="right">
      <SvgIcon>
        <EditIcon />
      </SvgIcon>
      <SvgIcon>
        <DeleteIcon />
      </SvgIcon>
    </TableCell>
  );
};

export default UserActions;
