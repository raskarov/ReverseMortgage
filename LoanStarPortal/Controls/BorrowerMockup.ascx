<%@ Import namespace="LoanStarPortal.Controls"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BorrowerMockup.ascx.cs" Inherits="BorrowerMockup" %>
<div style="padding:5px">
<table border="1" cellspacing="0" cellpadding="0" width="100%">
    <tr>
        <td style="width:100%;height:20px;color:White;background-color:Blue;font-size:16px;font-weight:bold" colspan="3" valign="top">
            &nbsp;Borrower Information
        </td>
    </tr>
    <tr>
        <td class="labeltd1" valign="top">&nbsp;</td>
        <td valign="top">&nbsp;</td>
        <td valign="top" align="right" style="padding-right:5px; height: 21px;"><span style="color:Blue; text-decoration:underline">Add Borrower</span></td>        
   </tr>
    <tr style="height:19px">
        <td valign="top">&nbsp;</td>
        <td valign="top" style="padding-left:5px;font-size:15px;font-weight:bold;">Borrower</td>
        <td valign="top" style="padding-left:5px;font-size:15px;font-weight:bold;">Co-Borrower</td>
        
    </tr>   
    <tr>
        <td valign="top" class="labeltd1">Salutation:</td>
        <td valign="top" class="controltd1">
                <select id="Select1">
                    <option>Mr.</option>
                    <option>Mrs.</option>
                    <option>Ms.</option>
                    <option>Miss.</option>
                </select>
        </td>
        <td valign="top" class="controltd2">
                <select id="Select2">
                    <option>Mr.</option>
                    <option>Mrs.</option>
                    <option>Ms.</option>
                    <option>Miss.</option>
                </select>
        </td>        
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">First Name:</td>
        <td valign="top" class="controltd1">
            <input id="Text1" type="text" value="Jonh" class="input1"/>
        </td>
        <td valign="top"  class="controltd2">
            <input id="Text2" type="text" class="input1"/>
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Middle Initial:</td>
        <td valign="top" class="controltd1">
            <input id="Text3" type="text" value="I" class="input2"/>
        </td>
        <td valign="top"  class="controltd2">
            <input id="Text4" type="text" class="input2"/>
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Last Name:</td>
        <td valign="top" class="controltd1">
            <input id="Text5" type="text" class="input1" value="Doe" />
        </td>
        <td valign="top" class="controltd1">
            <input id="Text6" type="text" class="input1"/>
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Marital Status:</td>
        <td valign="top" class="controltd1">
                <select id="Select3">
                    <option>Married</option>
                    <option>Unmaried</option>
                    <option>Widowed</option>
                </select>
        </td>
        <td valign="top" class="controltd2">
                <select id="Select4">
                    <option>Married</option>
                    <option>Unmaried</option>
                    <option>Widowed</option>
                </select>
        </td>        
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">AKA Names:</td>
        <td valign="top" class="controltd1">
            <input id="Text7" type="text" class="input1"/>
        </td>
        <td valign="top" class="controltd2">
            <input id="Text8" type="text" class="input1" />
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Gender:</td>
        <td valign="top" class="controltd1">
                <select id="Select5">
                    <option>Male</option>
                    <option>Female</option>
                </select>
        </td>
        <td valign="top" class="controltd2">
                <select id="Select6">
                    <option>Male</option>
                    <option>Female</option>
                </select>
        </td>        
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Birth Date:</td>
        <td valign="top" class="controltd1">
            <input id="Text9" type="text" value="10/22/1931"  class="input2"/>
        </td>
        <td valign="top"  class="controltd1">
            <input id="Text10" type="text"  class="input2"/>
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Actual Age:</td>
        <td valign="top" class="controltd1">
            <input id="Text11" type="text" class="input2" value=""/>
        </td>
        <td valign="top" class="controltd2">
            <input id="Text12" type="text"  class="input2"/>
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Nearest Age:</td>
        <td valign="top" class="controltd1">
            <input id="Text13" type="text" class="input2"/>
        </td>
        <td valign="top" class="controltd2">
            <input id="Text14" type="text" class="input2"/>
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Years at present address:</td>
        <td valign="top" class="controltd1">
            <input id="Text15" type="text" class="input2"/>
        </td>
        <td valign="top" class="controltd2">
            <input id="Text16" type="text" class="input2"/>
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Monthly Income:</td>
        <td valign="top" class="controltd1">
            <input id="Text17" type="text" class="input2"/>
        </td>
        <td valign="top" class="controltd1">
            <input id="Text18" type="text" class="input2"/>
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Real Estate Assets:</td>
        <td valign="top" class="controltd1">
            <input id="Text19" type="text"  class="input2"/>
        </td>
        <td valign="top" class="controltd1">
            <input id="Text20" type="text"  class="input2"/>
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Home Phone Number:</td>
        <td valign="top" class="controltd1">
            <input id="Text21" type="text"  class="input1"/>
        </td>
        <td valign="top" class="controltd2">
            <input id="Text22" type="text" class="input1"/>
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Check if different mailing address:</td>
        <td valign="top" >
            <input id="Checkbox1" type="checkbox" onclick="SetAddress(this.checked,'1');"/>
        </td>
        <td valign="top">
            <input id="Checkbox2" type="checkbox" onclick="SetAddress(this.checked,'2');"/>
        </td>
    </tr>    
    <tr id="tra1" style="display:none;">
        <td valign="top" class="labeltd2" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Address:</td>
        <td valign="top" class="controltd1">
            <input type="text" class="input1" id="address11" style="display:none;"/>
        </td>
        <td valign="top" class="controltd2">
            <input type="text" class="input1" id="address12" style="display:none;"/>
        </td>
    </tr>    
    <tr id="tra2" style="display:none;">
        <td valign="top" class="labeltd2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Address:</td>
        <td valign="top" class="controltd1">
            <input type="text" class="input1"  id="address21" style="display:none;"/>
        </td>
        <td valign="top" class="controltd2">
            <input type="text" class="input1" id="address22"  style="display:none;"/>
        </td>
    </tr>
    <tr id="tra3" style="display:none;">
        <td valign="top" class="labeltd2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;City:</td>
        <td valign="top" class="controltd1">
            <input id="city1" type="text" class="input1" style="display:none;"/>
        </td>
        <td valign="top" class="controltd1">
            <input id="city2" type="text" class="input1" style="display:none;"/>
        </td>
    </tr>    
    <tr id="tra4" style="display:none;">
        <td valign="top" class="labeltd2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;State:</td>
        <td valign="top" class="controltd1">
                <select id="state1"  style="display:none;">
                    <option>Alaska</option>
                    <option>Alabama</option>
                    <option>Arkansas</option>
                    <option>Arizona</option>
                    <option>California</option>
                    <option>Colorado</option>
                    <option>Connecticut</option>
                    <option>District Columbia</option>
                    <option>Delaware</option>
                    <option>Florida</option>
                    <option>Georgia</option>
                    <option>Hawaii</option>
                    <option>Iowa</option>
                    <option>Idaho</option>
                    <option>Illinois</option>
                    <option>Indiana</option>
                    <option>Kansas</option>
                    <option>Kentucky</option>
                    <option>Louisiana</option>
                    <option>Massachusetts</option>
                    <option>Maryland</option>
                    <option>Maine</option>
                    <option>Michigan</option>
                    <option>Minnesota</option>
                    <option>Missouri</option>
                    <option>Mississippi</option>
                    <option>Montana</option>
                    <option>North Carolina</option>
                    <option>North Dakota</option>
                    <option>Nebraska</option>
                    <option>New Hampshire</option>
                    <option>New Jersey</option>
                    <option>New Mexico</option>
                    <option>Nevada</option>
                    <option>New York</option>
                    <option>Oklahoma</option>
                    <option>Oregon</option>
                    <option>Pennsylvania</option>
                    <option>Rhode Island</option>
                    <option>South Carolina</option>
                    <option>R.D.</option>
                    <option>Tennessee</option>
                    <option>Texas</option>
                    <option>Utah</option>
                    <option>Virginia</option>
                    <option>Vermont</option>
                    <option>Washington</option>
                    <option>Wisconsin</option>
                    <option>West Virginia</option>
                    <option>Wyoming</option>
                    <option>Ohio</option>
                    <option>South Dakota</option>
                </select>
        </td>
        <td valign="top" class="controltd2">
                <select id="state2" style="display:none;">
                    <option>Alaska</option>
                    <option>Alabama</option>
                    <option>Arkansas</option>
                    <option>Arizona</option>
                    <option>California</option>
                    <option>Colorado</option>
                    <option>Connecticut</option>
                    <option>District Columbia</option>
                    <option>Delaware</option>
                    <option>Florida</option>
                    <option>Georgia</option>
                    <option>Hawaii</option>
                    <option>Iowa</option>
                    <option>Idaho</option>
                    <option>Illinois</option>
                    <option>Indiana</option>
                    <option>Kansas</option>
                    <option>Kentucky</option>
                    <option>Louisiana</option>
                    <option>Massachusetts</option>
                    <option>Maryland</option>
                    <option>Maine</option>
                    <option>Michigan</option>
                    <option>Minnesota</option>
                    <option>Missouri</option>
                    <option>Mississippi</option>
                    <option>Montana</option>
                    <option>North Carolina</option>
                    <option>North Dakota</option>
                    <option>Nebraska</option>
                    <option>New Hampshire</option>
                    <option>New Jersey</option>
                    <option>New Mexico</option>
                    <option>Nevada</option>
                    <option>New York</option>
                    <option>Oklahoma</option>
                    <option>Oregon</option>
                    <option>Pennsylvania</option>
                    <option>Rhode Island</option>
                    <option>South Carolina</option>
                    <option>R.D.</option>
                    <option>Tennessee</option>
                    <option>Texas</option>
                    <option>Utah</option>
                    <option>Virginia</option>
                    <option>Vermont</option>
                    <option>Washington</option>
                    <option>Wisconsin</option>
                    <option>West Virginia</option>
                    <option>Wyoming</option>
                    <option>Ohio</option>
                    <option>South Dakota</option>
                </select>
        </td>
    </tr>    
    <tr id="tra5" style="display:none;">
        <td valign="top" class="labeltd2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Zip:</td>
        <td valign="top" class="controltd1">
            <input id="zip1" type="text" class="input2" style="display:none;"/>
        </td>
        <td valign="top" class="controltd1">
            <input id="zip2" type="text" class="input2" style="display:none;"/>
        </td>
    </tr>
    <tr>
        <td style="width:100%;background-color:#C1D9F0;height:19px;font-weight:bold;font-size:15px;" colspan="3" valign="top">
            &nbsp;Declarations
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Are there any outstanding judgments against you?</td>
        <td valign="top" class="controltd1">
            <input id="Radio1" type="radio" name="r1" />Yes<input id="Radio2" type="radio" name="r1"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio3" type="radio" name="r2" />Yes<input id="Radio4" type="radio" name="r2"/>No
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Have you filed for any bankruptcy that has not been resolved?</td>
        <td valign="top" class="controltd1">
            <input id="Radio5" type="radio" name="r3" />Yes<input id="Radio6" type="radio" name="r3"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio7" type="radio" name="r4" />Yes<input id="Radio8" type="radio" name="r4"/>No
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Are you a party to a lawsuit?</td>
        <td valign="top" class="controltd1">
            <input id="Radio9" type="radio" name="r5" />Yes<input id="Radio10" type="radio" name="r5"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio11" type="radio" name="r6" />Yes<input id="Radio12" type="radio" name="r6"/>No
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Are you presently delinquent or in default on any federal debt?</td>
        <td valign="top" class="controltd1">
            <input id="Radio13" type="radio" name="r7" />Yes<input id="Radio14" type="radio" name="r7"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio15" type="radio" name="r8" />Yes<input id="Radio16" type="radio" name="r8"/>No
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Do you intend to occupy the property as your primary residence?</td>
        <td valign="top" class="controltd1">
            <input id="Radio17" type="radio" name="r9"/>Yes<input id="Radio18" type="radio" name="r9" />No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio19" type="radio" name="r10" />Yes<input id="Radio20" type="radio" name="r10"/>No
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Are you a co maker or endorser on a note?</td>
        <td valign="top" class="controltd1">
            <input id="Radio21" type="radio" name="r11" />Yes<input id="Radio22" type="radio" name="r11"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio23" type="radio" name="r12" />Yes<input id="Radio24" type="radio" name="r12"/>No
        </td>
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">Are you a US citizen?</td>
        <td valign="top" class="controltd1">
            <input id="Radio25" type="radio" name="r13" />Yes<input id="Radio26" type="radio" name="r13"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio27" type="radio" name="r14" />Yes<input id="Radio28" type="radio" name="r14"/>No
        </td>
    </tr>        
    <tr>
        <td valign="top" class="labeltd1">Are you a permanent resident alien?</td>
        <td valign="top" class="controltd1">
            <input id="Radio29" type="radio" name="r15" />Yes<input id="Radio30" type="radio" name="r15"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio31" type="radio" name="r16" />Yes<input id="Radio32" type="radio" name="r16"/>No
        </td>
    </tr>            
    <tr>
        <td style="width:100%;background-color:#C1D9F0;height:19px;font-weight:bold;font-size:15px;" colspan="3" valign="top">
            &nbsp;HMDA
        </td>
    </tr>        
    <tr>
        <td valign="top" class="labeltd1">Race:</td>
        <td valign="top" class="controltd1">
                <select id="Select9" style="width:100px">
                    <option></option>
                </select>
        </td>
        <td valign="top" class="controltd2">
                <select id="Select10" style="width:100px">
                    <option></option>
                </select>
        </td>        
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Ethnicity:</td>
        <td valign="top" class="controltd1">
                <select id="Select11" style="width:100px">
                    <option></option>
                </select>
        </td>
        <td valign="top" class="controltd2">
                <select id="Select12" style="width:100px">
                    <option></option>
                </select>
        </td>        
    </tr>    
    <tr>
        <td valign="top" class="labeltd1">I do not wish to furnish this information.</td>
        <td valign="top" class="controltd1">
            <input id="Checkbox3" type="checkbox" />
        </td>
        <td valign="top" class="controltd2">
            <input id="Checkbox4" type="checkbox" />
        </td>
    </tr>        
    <tr>
        <td style="width:100%;background-color:#C1D9F0;height:19px;font-weight:bold;font-size:15px;" colspan="3" valign="top">
            &nbsp;Power of Attorney
        </td>
    </tr>
    <tr>
        <td valign="top" class="labeltd1">Using Power of attorney?</td>
        <td valign="top" class="controltd1">
            <input id="Radio33" type="radio" name="r20" onclick="SetPOW(true,1);" />Yes<input id="Radio34" type="radio" name="r20" onclick="SetPOW(false,1);"/>No
        </td>
        <td valign="top" class="controltd2">
            <input id="Radio35" type="radio" name="r21" onclick="SetPOW(true,2);"/>Yes<input id="Radio36" type="radio" name="r21" onclick="SetPOW(false,2);"/>No
        </td>
    </tr>    
    <tr id="trp1">
        <td valign="top" class="labeltd1">Is it durable?</td>
        <td valign="top" class="controltd1">
            <span id="pow11"><input id="pow1y1" type="radio" name="r22" />Yes<input id="pow1n1" type="radio" name="r22"/>No</span>
        </td>
        <td valign="top" class="controltd2">
            <span id="pow12"><input id="pow1y2" type="radio" name="r23" />Yes<input id="pow1n2" type="radio" name="r23"/>No</span>
        </td>
    </tr>
    <tr id="trp2">
        <td valign="top" class="labeltd1">Does it allow for encumbering the subject property?</td>
        <td valign="top" class="controltd1">
            <span id="pow21"><input id="Radio41" type="radio" name="r24" />Yes<input id="Radio42" type="radio" name="r24"/>No</span>
        </td>
        <td valign="top" class="controltd2">
            <span id="pow22"><input id="Radio43" type="radio" name="r25" />Yes<input id="Radio44" type="radio" name="r25"/>No</span>
        </td>
    </tr>    
    <tr id="trp3">
        <td valign="top" class="labeltd1">Is it revocable?</td>
        <td valign="top" class="controltd1">
            <span id="pow31"><input id="Radio45" type="radio" name="r26" />Yes<input id="Radio46" type="radio" name="r26"/>No</span>
        </td>
        <td valign="top" class="controltd2">
            <span id="pow32"><input id="Radio47" type="radio" name="r27" />Yes<input id="Radio48" type="radio" name="r27"/>No</span>
        </td>
    </tr>
    <tr id="trp4">
        <td valign="top" class="labeltd1">Is the borrower incapacitated?</td>
        <td valign="top" class="controltd1">
            <span id="pow41"><input id="Radio49" type="radio" name="r28" />Yes<input id="Radio50" type="radio" name="r28"/>No</span>
        </td>
        <td valign="top" class="controltd2">
            <span id="pow42"><input id="Radio51" type="radio" name="r29" />Yes<input id="Radio52" type="radio" name="r29"/>No</span>
        </td>
    </tr>    
    <tr id="trp5">
        <td valign="top" class="labeltd1">What’s the date of execution?</td>
        <td valign="top" class="controltd1">
            <span id="pow51"><input id="Text31" type="text" class="input2"/></span>
        </td>
        <td valign="top" class="controltd2">
            <span id="pow52"><input id="Text32" type="text" class="input2"/></span>
        </td>
    </tr>    
 </table>
</div>
<div style='height:30px;'>&nbsp;</div>
