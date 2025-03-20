import os

def join_files(file_path, output_path):
    with open(output_path, 'wb') as output_file:
        chunk_number = 0
        while True:
            chunk_filename = f"{file_path}.part{chunk_number}"
            if not os.path.exists(chunk_filename):
                break
            with open(chunk_filename, 'rb') as chunk_file:
                output_file.write(chunk_file.read())
            os.remove(chunk_filename)
            chunk_number += 1

def process_directory(directory):
    for root, _, files in os.walk(directory):
        processed_files = set()
        for file in files:
            if '.part' in file:
                original_file = file.split('.part')[0]
                if original_file not in processed_files:
                    file_path = os.path.join(root, original_file)
                    join_files(file_path, file_path)
                    processed_files.add(original_file)

if __name__ == "__main__":
    process_directory(os.getcwd())
