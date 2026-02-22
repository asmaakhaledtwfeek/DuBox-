import json
import uuid
from datetime import datetime

def convert_ids_to_guid(input_file, output_file):
    """
    Converts integer IDs to GUIDs in the checklist hierarchy JSON file.
    Maintains all relationships between checklists, sections, and items.
    """
    
    # Read the input JSON file
    print(f"Reading {input_file}...")
    with open(input_file, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    # Create mapping dictionaries
    checklist_id_map = {}
    section_id_map = {}
    item_id_map = {}
    
    print("Converting IDs to GUIDs...")
    
    # Process checklists
    for checklist in data['checklists']:
        old_checklist_id = checklist['checklistId']
        new_checklist_id = str(uuid.uuid4())
        checklist_id_map[old_checklist_id] = new_checklist_id
        
        # Update checklist ID
        checklist['checklistId'] = new_checklist_id
        
        # Process sections
        for section in checklist.get('sections', []):
            old_section_id = section['checklistSectionId']
            new_section_id = str(uuid.uuid4())
            section_id_map[old_section_id] = new_section_id
            
            # Update section IDs
            section['checklistSectionId'] = new_section_id
            section['checklistId'] = new_checklist_id  # Update foreign key reference
            
            # Process items
            for item in section.get('items', []):
                old_item_id = item['predefinedItemId']
                new_item_id = str(uuid.uuid4())
                item_id_map[old_item_id] = new_item_id
                
                # Update item IDs
                item['predefinedItemId'] = new_item_id
                item['checklistSectionId'] = new_section_id  # Update foreign key reference
    
    # Write the output JSON file
    print(f"Writing converted data to {output_file}...")
    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    
    # Print statistics
    print(f"\nConversion completed successfully!")
    print(f"Total checklists converted: {len(checklist_id_map)}")
    print(f"Total sections converted: {len(section_id_map)}")
    print(f"Total items converted: {len(item_id_map)}")
    
    return data

if __name__ == "__main__":
    input_file = "checklist_hierarchy_fixed.json"
    output_file = "checklist_hierarchy_with_guids.json"
    
    try:
        convert_ids_to_guid(input_file, output_file)
        print(f"\n✓ Successfully created {output_file}")
    except Exception as e:
        print(f"\n✗ Error: {str(e)}")
        raise
