import { TableCell } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import SvgIcon from "@mui/material/SvgIcon";
import CustomButton from "../utils/CustomButtom";
import { Button } from "@mui/material";
const UserActions = ({ handleSelected }) => {
  return (
    <TableCell align="right">
      <CustomButton onClick={handleSelected} variant={"contained"} color="info">
        <SvgIcon>
          <EditIcon />
        </SvgIcon>
      </CustomButton>
    </TableCell>
  );
};

export default UserActions;
